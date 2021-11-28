from azure.cosmos import CosmosClient


import subprocess

RES_GROUP = "my-cosmsos-resource-group"
ACCT_NAME = "my-cosomso-account-name"


def get_account_uri(res_group, acct_name):
    completed = subprocess.run(
        f"az cosmosdb show --resource-group {res_group} --name {acct_name} --query documentEndpoint --output tsv",
        capture_output=True, shell=True, text=True)
    if completed.returncode != 0:
        raise ValueError(f"error getting URI")
    return completed.stdout


def get_key(res_group, acct_name):
    completed = subprocess.run(
        f"az cosmosdb list-keys --resource-group {res_group} --name {acct_name} --query primaryMasterKey --output tsv",
        capture_output=True, shell=True, text=True)
    if completed.returncode != 0:
        raise ValueError(f"error getting key")
    return completed.stdout


url = get_account_uri(RES_GROUP, ACCT_NAME)
key = get_key(RES_GROUP, ACCT_NAME)

print(url, key)
client = CosmosClient(url, credential=key)
