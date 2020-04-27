# Azure Dev Spaces setup

## Prerequisites

### Software
* K8s command-line tool **kubectl**: https://kubernetes.io/docs/tasks/tools/install-kubectl/
* Azure CLI: https://aka.ms/azcli
* Azure Dev Spaces client-side tools:  
https://docs.microsoft.com/en-us/azure/dev-spaces/how-to/install-dev-spaces#install-the-client-side-tools
	* Windows: https://aka.ms/get-azds-windows
	* Mac: https://aka.ms/get-azds-mac
	* Linux: https://aka.ms/get-azds-linux
	
Optional:
* [Visual Studio Code](https://code.visualstudio.com/) and [Azure CLI Tools](https://marketplace.visualstudio.com/items?itemName=ms-vscode.azurecli) extension.

## Create Azure Kubernetes Service

### Enable Helm 3 preview feature

Official docs:  
https://docs.microsoft.com/en-us/azure/dev-spaces/how-to/helm-3

1. Check if Helm3Preview is enabled:
	```shell
	az feature show --namespace Microsoft.DevSpaces --name Helm3Preview --output table
	```
	If `RegistrationState` is **Registered**, then the Helm3 support is turned ON, Skip next step.
2. Enable Helm3Preview feature:
	```shell
	az feature register --namespace Microsoft.DevSpaces --name Helm3Preview
	```
	Then try to check if Helm3Preview is enabled:
	```shell
	az feature show --namespace Microsoft.DevSpaces --name Helm3Preview --output table
	```
	Try check more times until you see the `RegistrationState` is **Registered**, Then refresh the registration of Microsoft.DevSpaces using `az provider register`:
	```shell
	az provider register --namespace Microsoft.DevSpaces
	```

### Create an AKS Service with Azure Dev Spaces enabled
1. Create AKS service:
    ```shell
    az aks create -g MyResourceGroupName -n MyAksServiceName --location SpecifiedAksSupportRegions --generate-ssh-keys
    ```
    Replace the above `MyResourceGroupName`, `MyAksServiceName`, `SpecifiedAksSupportRegions` with the desired Azure Resource Group Name, AKS Service Name, and [Azure Region that has Azure Dev Spaces support](https://azure.microsoft.com/en-us/global-infrastructure/services/?products=kubernetes-service).
2. Enable Azure Dev Spaces:
    ```shell
    az aks use-dev-spaces -g MyResourceGroupName -n MyAksServiceName
    ```
    ![Enable Azure Dev Spaces](./img/Apply_AzDs_on_AKS_02.png)
    Check if Azure Dev Spaces is enabled via kubectl:
    ```shell
    kubectl get deployment -n azds
    ```
    You should see `azds-webhook-deployment` & `traefik` deployment entries, but no `tiller` deployment.
    ![Use kubectl to see Azure Dev Spaces deployment status](./img/Check_if_AzDs_enabled.png)