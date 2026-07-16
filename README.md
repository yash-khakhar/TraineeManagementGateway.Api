## TraineeManagementGateway.Api  
A reverse proxy and API Gateway built with ASP.NET Core 10.0. This project serves as the single entry point for all client requests entering the TraineeManagement ecosystem on port 5000. It centralizes security cross-cutting concerns, handles routing mechanics, and decouples external clients from internal microservices. 

## 🛠 Role in Ecosystem Architecture  
The API Gateway provides a protective boundary around internal services. Instead of clients communicating directly with individual backends, they interact exclusively with the Gateway, which manages routing to downstream endpoints.  
```bash  
                        [ Client Requests ]
                                 │
                                 ▼ (Port 5000)
                     ┌─────────────────────┐
                     │ Auth Gateway (.api) │ ──► Centralized JWT Verification
                     └──────────┬──────────┘
                                │
            ┌───────────────────┴───────────────────┐
            ▼ (Downstream Routing)                  ▼ (Downstream Routing)
┌──────────────────────┐                ┌──────────────────────┐
│ TraineeManagement.api│                │ TrainingDirectory.Api│
└──────────────────────┘                └──────────────────────┘  
```  

## 🚀 Key Responsibilities  
* Centralized Authentication: Intercepts incoming client requests to validate JSON Web Tokens (JWT). Unauthorized traffic is rejected immediately at the perimeter before hitting internal microservice resource pools.  
* Request Routing & Reverse Proxying: Transparently maps and forwards external request patterns to the appropriate internal downstream microservice endpoints. 
* CORS Management: Centralizes Cross-Origin Resource Sharing policies to globally whitelist authorized frontend consumer applications in one configuration block.  
* System Abstraction: Prevents internal port assignments, component architecture, and physical service layouts from leaking out to public client clients.

## ⚙️ Tech Stack & Dependencies  
* Framework: C# and ASP.NET Core 10.0 Web API  
* Security Context: Microsoft.AspNetCore.Authentication.JwtBearer for perimeter token validation
* Reverse Proxy Routing: Configured YARP middleware
* Shared Contracts: References TraineeManagement.Shared for standardized data serialization models and DTO schemas  

## Running the Worker Locally  
1. Restore dependencies:  
```bash  
dotnet restore  
```  
2. Execute the project:  
```bash  
dotnet run  
```  

Once running, target your API testing tools (e.g., Postman or curl) at http://localhost:5000/api/Auth/register to begin interacting with the cluster securely.