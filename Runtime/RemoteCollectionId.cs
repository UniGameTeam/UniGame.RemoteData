namespace UniGame.RemoteData.FirebaseModule
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [InlineProperty]
    [Serializable]
    [ValueDropdown("@UniGame.RemoteData.RemoteEditorData.GetCollectionsId()")]
    public struct RemoteCollectionId
    {
        [SerializeField, HideInInspector] private string _value;

        public static implicit operator string(RemoteCollectionId v)
        {
            return v._value;
        }

        public static explicit operator RemoteCollectionId(string v)
        {
            return new RemoteCollectionId() {_value = v};
        }

        public override string ToString() => _value;

        public override int GetHashCode() => _value.GetHashCode();

        public RemoteCollectionId FromString(string value)
        {
            _value = value;
            return this;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RemoteCollectionId _))
                return false;
            return _value.Equals(((RemoteCollectionId) obj)._value);
        }
    }
}