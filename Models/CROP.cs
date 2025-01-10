using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace CallSummarizerDemo.Models;

public class CROP
{
    [JsonPropertyName("type")]
    public CROPType? Type { get; set; }
    
    [JsonPropertyName("trigger")]
    public string? Trigger { get; set; }
    
    [JsonPropertyName("recommendation")]
    public string? Recommendation { get; set; }
    
    [JsonPropertyName("evidence")] 
    public List<string>? Evidence { get; set; }
    
    [JsonPropertyName("policyNumberReference")]
    public string? PolicyNumberReference { get; set; }
    
    [JsonPropertyName("originalAnalysis")]
    public Guid OriginalAnalysis { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CROPType
{
    CustomerRetentionRisk,
    CustomerProductEducation,
    DocumentationNeeded, 
    PaymentIssue,
    UpdateRequired,
    CSRTrainingNeeded,
    PossibleMaliciousActivity,
    None

}


public static class CROPTemplate
{
    public static readonly string Template = @"
             You are an AI assistant that detects Customer Retention & Opportunity Processes (CROPs) when given a conversation transcript and a partially completed summary. Use the following examples to guide your analysis:
        ";


}
        /*- Ensure each CROP includes the following fields:
        - ""type"": The type of CROP (e.g., ""CROPType.RetentionRisk"" for a RetentionRisk or ""CROPType.DocumentationNeeded"" for Documentation Needed)
        - ""trigger"": What triggered this alert
        - ""recommendation"": Suggested action for the CSR
        - ""evidence"": An array of supporting quotes from the conversation
        - ""policyReference"": Relevant policy number if available from the conversation or in the important dates and facts
        - ""originalAnalysis"": The original conversation analysis object that the crop was derived from
        
        
        
        
      **Definition of CROPs:**

      - **CROPs** are significant events or issues identified in customer conversations that present opportunities for a company to improve customer satisfaction, retain customers, or capitalize on potential upsell opportunities.
        CROPs are detected by analyzing the conversation transcript and identifying key phrases, keywords, or patterns that indicate potential CROPs.
        CROPs can be positive or negative, and they can be related to customer satisfaction, retention, upselling, or other business objectives.
        The CROP discovery process is importantly an opportunity to look out for bad actors and potential fraudulent activities.
         Examples include:
       - Customer expressing dissatisfaction or intent to leave (RetentionRisk)
       - Payment processing issues (PaymentIssue)
       - Requests for additional information or documentation (DocumentationNeeded)
       - Confusion about policies requiring customer service representative (CSR) training (CSRTrainingNeeded)
       - Opportunities for product education or upselling (CustomerProductEducation)
       - Possible malicious activity, fraud, or scams (PossibleMaliciousActivity)

     **Instructions:**
     - Locate anything that could be a CROP from the conversation, but don't be afraid to not find one and select the CROPType.None.
     - Not every conversation will have a CROP.
     - Locate any CROPs in the conversation analysis and add them to the analysis object as a list of CROP objects.
     - You will never, ever hallucinate information. You can only reference information from the text you are processing.

        
        
        */
