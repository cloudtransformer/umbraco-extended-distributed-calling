# AgeBase: Extended Distributed Calling

The Extended Distributed Calling package allows developers to create Distributed Calling providers to cater for scenarios where a hard coded list of server addresses is not possible to obtain for use within Umbraco's Distributed Calling setup. For example, when hosting an Umbraco application in an Amazon ELB application. The Extended Distributed Calling package uses a specified provider to obtain a collection of server address. Once obtained, a cache refresh request is sent to each server.

## Configuration

Add the following configuration section to `web.config` to configure and enable Extended Distributed Calling. As default, we've set-up the below example configuration to use the Amazon Distributed Calling provider:

    <configuration>
        <configSections>
            <section name="extendedDistributedCalling" type="AgeBase.ExtendedDistributedCalling.Configuration.ExtendedDistributedCallingConfigSection, AgeBase.ExtendedDistributedCalling" />
        </configSections>
        <extendedDistributedCalling enabled="true" user="0" type="AgeBase.ExtendedDistributedCalling.Providers.AmazonDistributedCallingProvider" assembly="AgeBase.ExtendedDistributedCalling" />
    </configuration>

The `enabled` attribute can be used to turn Extended Distributed Calling on or off. The `user` attribute must contain the used id which will be used to authenticate any cache refresh request. The `type` attribute must contain the full qualified class name. The `assembly` attribute must contain the assembly name where the type resides. 

## Providers

To write your own provider, create a new class which implements the `IExtendedDistributedCallingProvider` interface. The interface contains one method which requires a list of addresses to be returned. To use your provider, change your the `type` and `assembly` configuration in web.config to the new provider. To get you started, the Extended Distributed Calling assembly contains the following providers for use within your Umbraco applications:

### Amazon Distributed Calling

For use on scalable Amazon Elastic Load Balanced applications. Type and assembly details are as follows:

  * Type: `AgeBase.ExtendedDistributedCalling.Providers.AmazonDistributedCallingProvider`
  * Assembly: `AgeBase.ExtendedDistributedCalling`

The provider requires the following Application Settings via web.config. These can either be manually or dynamically added via the AWS console:

  * `AWS_ACCESS_KEY_ID`
  * `AWS_SECRET_KEY`
  * `AWS_ENV_NAME`
  * `AWS_REGION`

The `AWS_ACCESS_KEY_ID` app setting must contain a valid access key to access the AWS API. The `AWS_SECRET_KEY` app setting must contain a valid secret key to access the AWS API. The `AWS_ENV_NAME` app setting must contain the name of the current ELB's environment. The `AWS_REGION` must contain the region of the current ELB's environment. The follow region values are accepted:

  * `us-east-1`
  * `us-west-1`
  * `us-west-2`
  * `eu-west-1`
  * `ap-northeast-1`
  * `ap-southeast-1`
  * `ap-southeast-2`
  * `sa-east-1`
  * `us-gov-west-1`
  * `cn-north-1`
  
__Note:__ For the Amazon Distributed Calling provider to work correctly, each EC2 instance attached to the load balancer must be able to communicate with all other attached EC2 instances. To allow this, the EC2 instance security group must allow incoming calls from the ELB security group on port 80. Allowing this rule does not open up each EC2 instance publicly.

## Contributing

To raise a new bug, create an [issue](https://github.com/agebase/umbraco-extended-distributed-calling/issues) on the Github repository. To fix a bug or add new features or providers, fork the repository and send a [pull request](https://github.com/agebase/umbraco-extended-distributed-calling/pulls) with your changes. Feel free to add ideas to the repository's [issues](https://github.com/agebase/umbraco-extended-distributed-calling/issues) list if you would to discuss anything related to the package.

## Publishing

Remember to include all necessary files within the package.xml file. Run the following script, entering the new version number when prompted to created a published version of the package:

    Build\Release.bat

The release script will amend all assembly versions for the package, build the solution and create the package file. The script will also commit and tag the repository accordingly to reflect the new version.