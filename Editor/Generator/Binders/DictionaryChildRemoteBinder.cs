using System;

namespace RemoteData.Editor.Generator.Binders
{
    [Serializable]
    public class DictionaryChildRemoteBinder : DictionaryRemoteBinder
    {
        public static string BindingTemplate =
            "var value%PROPERTY_NAME% = new MutableDictionary<%DICTIONARY_VALUE%>();\n\t\t\t" +
            "value%PROPERTY_NAME%.BindToSource(() => Object.%PROPERTY_NAME%,GetChildPath(nameof(Object.%PROPERTY_NAME%)), this);\n\t\t\t" +
            "%PROPERTY_NAME% = value%PROPERTY_NAME%;\n\t\t\t";
        
        private static string PropertyTemplate =
            "public IReactiveDictionary<string, %DICTIONARY_VALUE%> %PROPERTY_NAME% { get; private set; }";

        public override string GetBindingTemplate => BindingTemplate;

        public override string GetPropertyTemplate => PropertyTemplate;
    }
}