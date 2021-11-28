#!
RES_GROUP=my-cosmsos-resource-group
ACCT_NAME=my-cosomso-account-name

export ACCOUNT_URI=$(az cosmosdb show --resource-group $RES_GROUP --name $ACCT_NAME --query documentEndpoint --output tsv)
export ACCOUNT_KEY=$(az cosmosdb list-keys --resource-group $RES_GROUP --name $ACCT_NAME --query primaryMasterKey --output tsv)


echo $ACCOUNT_URI
echo $ACCOUNT_KEY
