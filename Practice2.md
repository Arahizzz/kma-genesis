AWS CLI v2 \
Create VPC

    aws ec2 create-vpc --cidr-block 10.10.0.0/18 \
        --no-amazon-provided-ipv6-cidr-block \
        --tag-specifications "ResourceType=vpc, Tags=[{Key=Name,Value=kma-genesis},{Key=Lesson,Value=public-clouds}]" \
        --query Vpc.VpcId \
        --output text

Save Vpc id \
vpc-id=vpc-0659023b7c0263d9b

Create subnet

    aws ec2 create-subnet --vpc-id="<vpc-id>" --cidr-block 10.10.1.0/24

Save subnet Id \
subnet-id=subnet-04207e9826b956234

Create other subnets

    aws ec2 create-subnet --vpc-id="<vpc-id>" --cidr-block 10.10.2.0/24

    aws ec2 create-subnet --vpc-id="<vpc-id>" --cidr-block 10.10.3.0/24

Create gateway

    aws ec2 create-internet-gateway --query InternetGateway.InternetGatewayId --output text

Save gateway id \
gateway-id=igw-03a3659bd6d1a1a85 \
Attach gateway 

    aws ec2 attach-internet-gateway --internet-gateway-id <gateway-id> --vpc-id <vpc-id>

Create launch template
1. Create file template.json
    
        "BlockDeviceMappings":[
            {
                "DeviceName":"/dev/xvda",
                "Ebs":{
                    "DeleteOnTermination": true,
                            "SnapshotId": "snap-089786b5ea5c7e96e",
                            "VolumeSize": 15,
                            "VolumeType": "gp2",
                            "Encrypted": false
                }
            }
        ],
        "ImageId":"ami-058e6df85cfc7760b",
        "InstanceType":"t2.micro"
        }

2. Run command:

         aws ec2 create-launch-template \
              --launch-template-name TaskTemplate \
              --launch-template-data file://template.json

Save launch template id \
launch-id=lt-0d998d7d4c155538a

Create autoscaling group

    aws autoscaling create-auto-scaling-group \
    --auto-scaling-group-name "auto-scaling-group" \
    --min-size=1 \
    --max-size=3 --vpc-zone-identifier=<subnet-id> \
    --launch-template "LaunchTemplateId=<launch-id>"

Create security group

    aws ec2 create-security-group --group-name my-sg --description "My security group" --vpc-id <vpc-id>

Save security group id \
sec-id=sg-0d189d5f6d6f1e6db

Open ports
```
  aws ec2 authorize-security-group-ingress --group-id <sec-id> --protocol tcp --port 22
  aws ec2 authorize-security-group-ingress --group-id <sec-id> --protocol tcp --port 80
  aws ec2 authorize-security-group-ingress --group-id <sec-id> --protocol tcp --port 443
```

Create load balancer

    aws elb create-load-balancer --load-balancer-name my-load-balancer \
    --listeners "Protocol=HTTP,LoadBalancerPort=80,InstanceProtocol=HTTP,InstancePort=80" \
    --subnets <subnet-id> --security-groups <sec-id>

