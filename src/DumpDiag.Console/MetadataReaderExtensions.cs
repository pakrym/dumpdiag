using System;
using System.Collections.Immutable;
using System.Reflection.Metadata;
using Microsoft.Diagnostics.Runtime;

namespace DumpDiag.Console
{
    static class MetadataReaderExtensions
    {
        // This has to be a service and be cached

        public static MetadataReader CreateMetadataReader(this ClrModule module)
        {
            if (module.MetadataAddress == 0)
            {
                return null;
            }

            var d = new byte[module.MetadataLength];

            module.Runtime.ReadMemory(module.MetadataAddress, d, d.Length, out var read);
            return MetadataReaderProvider.FromMetadataImage(ImmutableArray.Create(d, 0, read)).GetMetadataReader();
        }

        public static CustomAttributeValue<string> GetAssemblyAttributeStringValue(this MetadataReader reader, string name)
        {
            if (reader != null)
            {
                foreach (var readerCustomAttribute in reader.CustomAttributes)
                {
                    var ca = reader.GetCustomAttribute(readerCustomAttribute);
                    string typeName;
                    if (ca.Constructor.Kind == HandleKind.MethodDefinition)
                    {
                        var ctor = reader.GetMethodDefinition((MethodDefinitionHandle)ca.Constructor);
                        var type = reader.GetTypeDefinition(ctor.GetDeclaringType());
                        typeName = reader.GetString(type.Name);
                    }
                    else
                    {
                        var memberreference = reader.GetMemberReference((MemberReferenceHandle)ca.Constructor);
                        var tr = reader.GetTypeReference(((TypeReferenceHandle)memberreference.Parent));
                        typeName = reader.GetString(tr.Name);

                    }
                    if (typeName != name)
                    {
                        continue;
                    }
                    return ca.DecodeValue(new ValueProvider());
                }
            }
            return default(CustomAttributeValue<string>);
        }


        internal class ValueProvider : ICustomAttributeTypeProvider<string>
        {
            public string GetPrimitiveType(PrimitiveTypeCode typeCode)
            {
                return string.Empty;
            }

            public string GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
            {
                throw new NotImplementedException();
            }

            public string GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
            {
                throw new NotImplementedException();
            }

            public string GetSZArrayType(string elementType)
            {
                throw new NotImplementedException();
            }

            public string GetSystemType()
            {
                throw new NotImplementedException();
            }

            public string GetTypeFromSerializedName(string name)
            {
                throw new NotImplementedException();
            }

            public PrimitiveTypeCode GetUnderlyingEnumType(string type)
            {
                throw new NotImplementedException();
            }

            public bool IsSystemType(string type)
            {
                throw new NotImplementedException();
            }
        }
    }
}