# smash-tennis-api-azfunc-cosmos
Visual Studio solution.
A NET v3 Azure Functions backend using Azure Cosmos DB as the repository.

## App settings
Use secrets.json for Cosmos DB connections seetings. They must be in a section as follows:
```
  "CosmosDb": {
    "Account": "{{ Endpoint URI }},
    "Key": "{{ Primary Key }}",
    "DatabaseId": "{{ Database ID / Name }}"
  }
  ```
  
## Update requests
Update endpoints such as 'PUT /{resource}/id' assume the whole resource with **all** its properties is being provided in the body.
Properties not provided will not be mappped by the JSON converter and therefore will have a default value (most of times, a 'null' value) and that is what will be sent to the database.

Should you need to perform a partial update, use PATCH request. Example: 'PATCH /{resource}/id'.
The body needs to be in the form of a 'Microsoft.AspNetCore.JsonPatch.JsonPatchDocument':

```javascript
[
    {
      "op": "replace",
      "path": "/lastName",
      "value": "Doe"
    }
]
```
Note the body is an array of operations, meaning it's posible to update more than one property wtih a single PATCH request.

