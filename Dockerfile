FROM microsoft/dotnet:2.0-sdk


# copy and build everything
ADD deploy/xdebian-64 ./app

WORKDIR app


EXPOSE 8080
EXPOSE 8085
ENTRYPOINT ["dotnet", "hostBatch.dll"]
