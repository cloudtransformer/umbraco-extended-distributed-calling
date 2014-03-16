using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AgeBase.ExtendedDistributedCalling.Interfaces;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.ElasticBeanstalk;
using Amazon.ElasticBeanstalk.Model;
using Amazon.ElasticLoadBalancing;
using Amazon.ElasticLoadBalancing.Model;

namespace AgeBase.ExtendedDistributedCalling.Providers
{
    public class AmazonDistributedCallingProvider : IExtendedDistributedCallingProvider
    {
        public List<string> GetServers()
        {
            if (ConfigurationManager.AppSettings["AWS_ACCESS_KEY_ID"] == null)
                throw new ArgumentException("Missing AWS_ACCESS_KEY_ID app setting");

            if (ConfigurationManager.AppSettings["AWS_SECRET_KEY"] == null)
                throw new ArgumentException("Missing AWS_SECRET_KEY app setting");

            if (ConfigurationManager.AppSettings["AWS_ENV_NAME"] == null)
                throw new ArgumentException("Missing AWS_ENV_NAME app setting");

            if (ConfigurationManager.AppSettings["AWS_REGION"] == null)
                throw new ArgumentException("Missing AWS_REGION app setting");

            var accessKey = ConfigurationManager.AppSettings["AWS_ACCESS_KEY_ID"];
            var secretKey = ConfigurationManager.AppSettings["AWS_SECRET_KEY"];
            var environmentName = ConfigurationManager.AppSettings["AWS_ENV_NAME"];

            RegionEndpoint regionEndpoint = null;

            switch (ConfigurationManager.AppSettings["AWS_REGION"].Trim().ToLower())
            {
                case "us-east-1":
                    regionEndpoint = RegionEndpoint.USEast1;
                    break;
                case "us-west-1":
                    regionEndpoint = RegionEndpoint.USWest1;
                    break;
                case "us-west-2":
                    regionEndpoint = RegionEndpoint.USWest2;
                    break;
                case "eu-west-1":
                    regionEndpoint = RegionEndpoint.EUWest1;
                    break;
                case "ap-northeast-1":
                    regionEndpoint = RegionEndpoint.APNortheast1;
                    break;
                case "ap-southeast-1":
                    regionEndpoint = RegionEndpoint.APSoutheast1;
                    break;
                case "ap-southeast-2":
                    regionEndpoint = RegionEndpoint.APSoutheast2;
                    break;
                case "sa-east-1":
                    regionEndpoint = RegionEndpoint.SAEast1;
                    break;
                case "us-gov-west-1":
                    regionEndpoint = RegionEndpoint.USGovCloudWest1;
                    break;
                case "cn-north-1":
                    regionEndpoint = RegionEndpoint.CNNorth1;
                    break;
            }

            if (regionEndpoint == null)
                throw new ArgumentException("Incorrect AWS_REGION endpoint");

            // Create client
            var elasticBeanstalkClient = new AmazonElasticBeanstalkClient(accessKey, secretKey, regionEndpoint);

            // Get environment resources for environment
            var environmentResourcesRequest = new DescribeEnvironmentResourcesRequest { EnvironmentName = environmentName };
            var resourceResponse = elasticBeanstalkClient.DescribeEnvironmentResources(environmentResourcesRequest);

            // Create ELB client
            var elasticLoadBalancingClient = new AmazonElasticLoadBalancingClient(accessKey, secretKey, regionEndpoint);

            // Get load balancers for all environment's load balancers
            var loadBalancersRequest = new DescribeLoadBalancersRequest();

            foreach (var loadBalancer in resourceResponse.EnvironmentResources.LoadBalancers)
                loadBalancersRequest.LoadBalancerNames.Add(loadBalancer.Name);

            var describeLoadBalancersResponse = elasticLoadBalancingClient.DescribeLoadBalancers(loadBalancersRequest);

            // Create EC2 client
            var ec2Client = new AmazonEC2Client(accessKey, secretKey, regionEndpoint);

            // Get instances for all instance ids in all load balancers
            var instancesRequest = new DescribeInstancesRequest();

            // Get all instance ids for all load balancers
            foreach (var instance in describeLoadBalancersResponse.LoadBalancerDescriptions.SelectMany(loadBalancer => loadBalancer.Instances))
                instancesRequest.InstanceIds.Add(instance.InstanceId);

            var instancesResponse = ec2Client.DescribeInstances(instancesRequest);

            // Find all private dns names
            return (from reservation in instancesResponse.Reservations from instance in reservation.Instances select instance.PrivateDnsName).ToList();
        }
    }
}