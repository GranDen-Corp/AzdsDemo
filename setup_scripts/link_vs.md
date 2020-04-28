# Visual Studio link to existing Azure Dev Spaces

Official docs:   
https://docs.microsoft.com/en-us/azure/dev-spaces/get-started-netcore-visualstudio

## Prerequisites

### Software

* Visual Studio Kubernetes Tools:   
https://docs.microsoft.com/en-us/visualstudio/containers/tutorial-kubernetes-tools   
![Visual Studio 2019 Kubernetes tools install](./img/VS2019_K8S_tools.png)

## Link to existing Azure Dev Spaces

1. Open Project Property Debug Page, select **Azure Dev Spaces**:   
    ![Switch to Azure Dev Spaces](./img/Select_Azure_Dev_Spaces.png)
2. Select the AKS service that Azure Dev Spaces enabled, then select or create a namespace:
    ![](./img/Select_Azure_Dev_Spaces_done.png)
3. Be sure to modify `azds.yml` file to match the custom domain name and HTTPS SSL certificate if the Azure Dev Space has config [that](./service_create.md):   
    ![Modify azds.yml for Custom Domain & HTTPS](./img/Update_azds_yml_for_custom_domain.png)
