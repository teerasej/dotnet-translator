

using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

var appConfig = new ConfigurationBuilder()
        .AddUserSecrets<Program>()
        .Build();

if (string.IsNullOrEmpty(appConfig["translator-api-key"]))
{
    Console.WriteLine("Missing or invalid appsettings.json...exiting");
    return;
}


String subscriptionKey = appConfig["translator-api-key"];
String endpoint = "https://api.cognitive.microsofttranslator.com/";

// Add your location, also known as region. The default is global.
// This is required if using a Cognitive Services resource.
String location = "southeastasia";


Console.WriteLine("Translate from language:");
string translateFromLang = Console.ReadLine();

Console.WriteLine("Translate to language:");
string tranlateToLang = Console.ReadLine();


Console.WriteLine("Input message:");
string textToTranslate = Console.ReadLine();

// Input and output languages are defined as parameters.
string route = $"/translate?api-version=3.0&from={translateFromLang}&to={tranlateToLang}";
object[] body = new object[] { new { Text = textToTranslate } };
var requestBody = JsonConvert.SerializeObject(body);

using (var client = new HttpClient())
using (var request = new HttpRequestMessage())
{
    // Build the request.
    request.Method = HttpMethod.Post;
    request.RequestUri = new Uri(endpoint + route);
    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
    request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
    request.Headers.Add("Ocp-Apim-Subscription-Region", location);

    // Send the request and get response.
    HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
    // Read response as a string.
    string result = await response.Content.ReadAsStringAsync();
    Console.WriteLine(result);
}