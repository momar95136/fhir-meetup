// See https://aka.ms/new-console-template for more information
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Utility;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using System.Threading.Tasks;

var location = "/Users/omarm5/projects/synthea/output/fhir";
FhirClient client = new FhirClient("http://ec2-54-186-238-19.us-west-2.compute.amazonaws.com:8080/fhir");

// read Hospital Information
var hospitalFile = Directory.GetFiles(location, "*hospital*")[0];
var bundle = new Hl7.Fhir.Serialization.FhirJsonParser().Parse<Bundle>(File.ReadAllText(hospitalFile));

if (bundle != null)
{
    await client.TransactionAsync(bundle);
}


// read Hospital Information
var practFile = Directory.GetFiles(location, "*practitioner*")[0];
bundle = new Hl7.Fhir.Serialization.FhirJsonParser().Parse<Bundle>(File.ReadAllText(practFile));
if (bundle != null)
{
    await client.TransactionAsync(bundle);
}


foreach (var f in Directory.GetFiles(location))
{
    if (f == hospitalFile || f == practFile) continue;

    Console.WriteLine($"Procesing file ......{f}");
    bundle = new Hl7.Fhir.Serialization.FhirJsonParser().Parse<Bundle>(File.ReadAllText(f));
    if (bundle != null)
    {
        await client.TransactionAsync(bundle);
    }


}

