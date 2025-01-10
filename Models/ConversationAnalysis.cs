using System.Text.Json.Serialization;

namespace CallSummarizerDemo.Models;
public class ConversationAnalysis
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonPropertyName("comprehensiveCallSummary")]
    public string? ComprehensiveCallSummary { get; set; }

    [JsonPropertyName("keyPoints")]
    public List<string>? KeyPoints { get; set; }

    // Enhanced fact tracking with categorization
    [JsonPropertyName("importantFacts")]
    public List<FactItem>? ImportantFacts { get; set; }

    [JsonPropertyName("importantNumbers")]
    public List<NumberItem>? ImportantNumbers { get; set; }

    [JsonPropertyName("importantDates")]
    public List<DateItem>? ImportantDates { get; set; }

    // Enhanced policy tracking
    [JsonPropertyName("policies")]
    public List<PolicyInfo>? Policies { get; set; }

    // Enhanced address tracking
    [JsonPropertyName("addresses")]
    public AddressChanges? Addresses { get; set; }

    // Enhanced reference number tracking
    [JsonPropertyName("referenceNumbers")]
    public List<ReferenceNumber>? ReferenceNumbers { get; set; }

    [JsonPropertyName("singleSentenceSummary")]
    public string? SingleSentenceSummary { get; set; }

    [JsonPropertyName("conversationTone")]
    public string? ConversationTone { get; set; }

    [JsonPropertyName("reasonForCalling")]
    public string? ReasonForCalling { get; set; }

    [JsonPropertyName("solutionProposed")]
    public bool? SolutionProposed { get; set; }

    [JsonPropertyName("offeredSolution")]
    public string? OfferedSolution { get; set; }

    // Enhanced customer information
    [JsonPropertyName("customer")]
    public CustomerInfo? Customer { get; set; }

    // Legacy fields maintained for backwards compatibility
    [JsonPropertyName("customerFirstandLastName")]
    public string? CustomerFirstAndLastName { get; set; }

    [JsonPropertyName("customerPhoneNumber")]
    public string? CustomerPhoneNumber { get; set; }

    [JsonPropertyName("customerEmail")]
    public string? CustomerEmail { get; set; }

    [JsonPropertyName("policyNumber")]
    public string? PolicyNumber { get; set; }

    [JsonPropertyName("paymentInformation")]
    public PaymentInfo? PaymentInformation { get; set; }

    // Legacy payment fields maintained for backwards compatibility
    [JsonPropertyName("paymentDate")]
    public string? PaymentDate { get; set; }

    [JsonPropertyName("paymentAmount")]
    public string? PaymentAmount { get; set; }

    [JsonPropertyName("lateFees")]
    public string? LateFees { get; set; }

    [JsonPropertyName("overdraftFee")]
    public string? OverdraftFee { get; set; }

    [JsonPropertyName("crop")]
    public CROP? Crop { get; set; }

    // Confidence scoring for the analysis
    [JsonPropertyName("confidenceScores")]
    public ConfidenceScores? ConfidenceScores { get; set; }
}

// Enhanced models for better information organization
public class PolicyInfo
{
    [JsonPropertyName("policyNumber")]
    public string PolicyNumber { get; set; } = string.Empty;

    [JsonPropertyName("policyType")]
    public string PolicyType { get; set; } = string.Empty;

    [JsonPropertyName("policyAmount")]
    public decimal? PolicyAmount { get; set; }

    [JsonPropertyName("policyHolder")]
    public string PolicyHolder { get; set; } = string.Empty;

    [JsonPropertyName("startDate")]
    public DateTime? StartDate { get; set; }

    [JsonPropertyName("beneficiaries")]
    public BeneficiaryInfo Beneficiaries { get; set; } = new();

    [JsonPropertyName("premiumInformation")]
    public PremiumInfo? PremiumInformation { get; set; }
}

public class PremiumInfo
{
    [JsonPropertyName("premium")]
    public decimal Premium { get; set; }

    [JsonPropertyName("premiumType")]
    public string PremiumType { get; set; } = string.Empty;
}

public class BeneficiaryInfo
{
    [JsonPropertyName("primary")]
    public List<string> Primary { get; set; } = new();

    [JsonPropertyName("contingent")]
    public List<string> Contingent { get; set; } = new();

    [JsonPropertyName("intendedChanges")]
    public List<BeneficiaryChange> IntendedChanges { get; set; } = new();
}

public class BeneficiaryChange
{
    [JsonPropertyName("date")]
    public DateTime? ChangeDate { get; set; }

    [JsonPropertyName("requestedBy")]
    public string RequestedBy { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("intendedBeneficiaries")]
    public List<string> IntendedBeneficiaries { get; set; } = new();
}

public class AddressChanges
{
    [JsonPropertyName("previous")]
    public List<AddressHistory> Previous { get; set; } = new();

    [JsonPropertyName("current")]
    public Address? Current { get; set; }

    [JsonPropertyName("future")]
    public AddressChange? Future { get; set; }
}

public class Address
{
    [JsonPropertyName("street")]
    public string Street { get; set; } = string.Empty;

    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("zipCode")]
    public string ZipCode { get; set; } = string.Empty;
}

public class AddressHistory : Address
{
    [JsonPropertyName("validUntil")]
    public DateTime? ValidUntil { get; set; }
}

public class AddressChange : Address
{
    [JsonPropertyName("effectiveDate")]
    public DateTime? EffectiveDate { get; set; }
}

public class PaymentInfo
{
    [JsonPropertyName("bankInformation")]
    public BankInfo? BankInformation { get; set; }

    [JsonPropertyName("draftDate")]
    public int? DraftDay { get; set; }

    [JsonPropertyName("premiumAmount")]
    public decimal? PremiumAmount { get; set; }

    [JsonPropertyName("changes")]
    public List<PaymentChange> Changes { get; set; } = new();
}

public class BankInfo
{
    [JsonPropertyName("routingNumber")]
    public string RoutingNumber { get; set; } = string.Empty;

    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("accountType")]
    public string AccountType { get; set; } = string.Empty;
}

public class PaymentChange
{
    [JsonPropertyName("type")]
    public string ChangeType { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("effectiveDate")]
    public DateTime? EffectiveDate { get; set; }

    [JsonPropertyName("reason")]
    public string Reason { get; set; } = string.Empty;
}

public class CustomerInfo
{
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("relationship")]
    public string? Relationship { get; set; }
}

public class ReferenceNumber
{
    [JsonPropertyName("number")]
    public string Number { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public DateTime? Date { get; set; }
}

public class ConfidenceScores
{
    [JsonPropertyName("overallConfidence")]
    public decimal OverallConfidence { get; set; }

    [JsonPropertyName("factExtraction")]
    public decimal FactExtraction { get; set; }

    [JsonPropertyName("policyInformation")]
    public decimal PolicyInformation { get; set; }

    [JsonPropertyName("customerInformation")]
    public decimal CustomerInformation { get; set; }
}

// Original support classes with minor enhancements
public class FactItem
{
    [JsonPropertyName("fact")]
    public string Fact { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("confidence")]
    public decimal? Confidence { get; set; }
}

public class NumberItem
{
    [JsonPropertyName("number")]
    public string Number { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("confidence")]
    public decimal? Confidence { get; set; }
}

public class DateItem
{
    [JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("confidence")]
    public decimal? Confidence { get; set; }
}

