dotnet restore src/PostTaskFunction
dotnet lambda package -pl src/PostTaskFunction --configuration Release --framework net6.0 --output-package src/PostTaskFunction/bin/Release/net6.0/PostTaskFunction.zip
dotnet restore src/ListTaskFunction
dotnet lambda package -pl src/ListTaskFunction --configuration Release --framework net6.0 --output-package src/ListTaskFunction/bin/Release/net6.0/ListTaskFunction.zip
dotnet restore src/GetTaskFunction
dotnet lambda package -pl src/GetTaskFunction --configuration Release --framework net6.0 --output-package src/GetTaskFunction/bin/Release/net6.0/GetTaskFunction.zip

