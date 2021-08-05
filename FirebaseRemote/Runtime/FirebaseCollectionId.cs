namespace UniGame.RemoteData.FirebaseModule
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [InlineProperty]
    [Serializable]
    [ValueDropdown("@UniGame.RemoteData.FirebaseModule.FirebaseEditorData.GetCollectionsId()")]
    public struct FirebaseCollectionId
    {
        [SerializeField, HideInInspector] private string _value;

        public static implicit operator string(FirebaseCollectionId v)
        {
            return v._value;
        }

        public static explicit operator FirebaseCollectionId(string v)
        {
            return new FirebaseCollectionId() {_value = v};
        }

        public override string ToString() => _value;

        public override int GetHashCode() => _value.GetHashCode();

        public FirebaseCollectionId FromString(string value)
        {
            _value = value;
            return this;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FirebaseCollectionId _))
                return false;
            return _value.Equals(((FirebaseCollectionId) obj)._value);
        }
    }
}