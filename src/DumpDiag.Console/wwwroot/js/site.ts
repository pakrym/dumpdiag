let nextBlockId = 0;

console.log("started");

document.addEventListener("DOMContentLoaded", () => {
    new Application().start();
});

class Application {
    private host: HTMLDivElement;
    private blockTemplate: HTMLTemplateElement;

    constructor() {
        this.host = document.querySelector("#block-host");
        if (!this.host) {
            throw new Error("Failed to find host element: '#block-host'");
        }

        this.blockTemplate = document.querySelector("#t-block");
        if (!this.blockTemplate) {
            throw new Error("Failed to find block template element: '#t-block'");
        }
    }

    start() {
        this.bind(document.body);

        var commandInput = document.getElementById("command") as HTMLInputElement;
        commandInput.addEventListener("keypress", async (e: KeyboardEvent) => {
            var parts = commandInput.value.split(" ", 2);
            if (parts.length === 0) {
                return;
            }
            var command = "";
            var commandArguments = "";
            if (parts.length > 0) {
                command = parts[0];
            }
            if (parts.length > 1) {
                commandArguments = parts[1];
            }

            if (e.keyCode === 13) {
                await this.runCommandUrl(`/Commands?command=${command}&arguments=${commandArguments}`);
            }
        });
    }

    bind(root: HTMLElement) {
        root.querySelectorAll("[data-action]").forEach((e) => {
            this.bindAction(e as HTMLElement);
        });
    }

    bindAction(elem: HTMLElement) {
        elem.onclick = async () => {
            let action = elem.dataset.action;

            console.log(`Running ${action} action...`);
            switch (action) {
                case "command":
                    await this.runCommand(elem);
                    break;
                case "remove":
                    this.runRemove(elem);
                    break;
            }
            console.log(`Finished ${action} action.`);
        };
        console.log(`Bound action handler for ${elem.id}`);
    }

    runRemove(elem: HTMLElement) {
        let toRemove = elem.closest("[data-removable]");
        toRemove.parentElement.removeChild(toRemove);
    }

    async runCommand(elem: HTMLElement) {
        let url = elem.dataset.href;
        if (isAnchor(elem)) {
            if (!url) {
                url = elem.href;
            }
            elem.href = "#";
        }

        if (!url) {
            throw new Error("Element doesn't have an 'href' or 'data-href' attribute.");
        }
        await this.runCommandUrl(url);
    }

    async runCommandUrl(url: string) {
        console.log(`Fetching ${url} ...`);
        let resp = await fetch(url);
        console.log(`Received result from  ${url}.`);

        // Instantiate the template
        let block = document.importNode(this.blockTemplate.content, true).querySelector(".block") as HTMLElement;
        block.id = `block-${nextBlockId}`;
        block.dataset["blockid"] = nextBlockId.toString();
        nextBlockId += 1;

        let content = block.querySelector(".block-body");
        content.innerHTML = await resp.text();

        // Bind handlers
        this.bind(block);

        // Insert the content into the host
        this.host.prepend(block);
    }
}

function isAnchor(elem: Element): elem is HTMLAnchorElement {
    return elem.tagName === "A";
}
