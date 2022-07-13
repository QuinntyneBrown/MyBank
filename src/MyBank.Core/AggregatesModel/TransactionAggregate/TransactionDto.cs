using System;

namespace MyBank.Core
{
    public class TransactionDto
    {
        public Guid? TransactionId { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
    }
}
