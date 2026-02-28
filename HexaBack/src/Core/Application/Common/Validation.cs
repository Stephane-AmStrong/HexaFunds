namespace Application.Common;

public static class Validation
{
    public static class Messages
    {
        public const string EntityNotFound = "{0} with id '{{PropertyValue}}' does not exist";
        public const string FieldRequired = "{{PropertyName}} is required";
        public const string FieldAlreadyInUseByAnother = "{0} '{{PropertyValue}}' is already used by another {1}";
        public const string RelationshipAlreadyExists = "A {0} between this {1} '{2}' and {3} '{4}' already exists";
        public const string FieldCannotBeModifiedAfterCreation = "The {0} cannot be modified after creation. Please keep it original value";
        public const string Field1AndField2AlreadyInUse = "{0} '{1}' and {2} '{3}' are already in use";
    }

    public static class Entities
    {
        public const string CheckingAccount = "CheckingAccount";
        public const string SavingsAccount = "SavingsAccount";
        public const string Transaction = "Transaction";
        public const string BankAccount = "BankAccount";
    }
}
