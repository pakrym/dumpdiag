var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
let nextBlockId = 0;
console.log("started");
document.addEventListener("DOMContentLoaded", () => {
    new Application().bind(document.body);
});
class Application {
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
    bind(root) {
        root.querySelectorAll("[data-action]").forEach((e) => {
            this.bindAction(e);
        });
    }
    bindAction(elem) {
        elem.onclick = () => __awaiter(this, void 0, void 0, function* () {
            let action = elem.dataset.action;
            console.log(`Running ${action} action...`);
            switch (action) {
                case "command":
                    yield this.runCommand(elem);
                    break;
                case "remove":
                    this.runRemove(elem);
                    break;
            }
            console.log(`Finished ${action} action.`);
        });
        console.log(`Bound action handler for ${elem.id}`);
    }
    runRemove(elem) {
        let toRemove = elem.closest("[data-removable]");
        toRemove.parentElement.removeChild(toRemove);
    }
    runCommand(elem) {
        return __awaiter(this, void 0, void 0, function* () {
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
            console.log(`Fetching ${url} ...`);
            let resp = yield fetch(url);
            console.log(`Received result from  ${url}.`);
            // Instantiate the template
            let block = document.importNode(this.blockTemplate.content, true).querySelector(".block");
            block.id = `block-${nextBlockId}`;
            block.dataset["blockid"] = nextBlockId.toString();
            nextBlockId += 1;
            let content = block.querySelector(".block-body");
            content.innerHTML = yield resp.text();
            // Bind handlers
            this.bind(block);
            // Insert the content into the host
            this.host.prepend(block);
        });
    }
}
function isAnchor(elem) {
    return elem.tagName === "A";
}
//# sourceMappingURL=site.js.map