# EmployeeProfileManagement

## High Level Architecture
![image](https://user-images.githubusercontent.com/21975202/233886288-419f5031-4fe7-4083-be9c-c41a7e36efc2.png)

## Dependencies
 - Azure SQL DB
 - Azure Key Vault

### Scripts
Database script to create necessary tables are in /db scripts

### Setup
#### API
1. Configure AZURE_CONNECTION_STRING in appSettings.json OR 
2. Configure AzureKeyVaultName to your key vault

### Web App
1. Run EF migration
2. Configure Connection String to your Identity database

### Tests
- EmployeeProfile.postman_collection is provided with examples

### TODO
- Add features to store images in Azure SQL / Azure Blob Storage
- Add unit tests
- Update Open API specification for richer developer experience
