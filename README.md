# smash-tennis-api-azfunc-cosmos
Backend; .NET v3 Azure Functions using Cosmos DB

## App settings
Use secrets.json for Cosmos DB connections seetings. They must be in a section as follows:
```
  "CosmosDb": {
    "Account": "{{ Endpoint URI }},
    "Key": "{{ Primary Key }}",
    "DatabaseId": "{{ Database ID / Name }}"
  }
  ```
  
