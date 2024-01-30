# E-Commerce API

## Overview
E-Commerce Web API, encompassing user authentication, product CRUD operations, and essential user actions like cart management, 
checkout, and selling. Integrated a responsive email notification system for enhanced user 
communication.


## Technologies Used
ASP.NET Core ,
C# ,
Entity Framework Core and
SQL Server 

## Key Features

### Authentication and Authorization
- API is secured using Identity, Jwt and refresh tokens 
- Define roles and permissions for users to ensure proper authorization

### CRUD Operations
- Create, Read, Update, and Delete operations for Products
- Utilize RESTful principles to design consistent and predictable endpoints

### Design Patterns 
- (Repository pattern and Unit of work) is used to handle database connections 

### Admins/Owners Features  	
- add/delete/update/show the products

### Users Features 
-  adding/removing product to his cart
-  viewing his cart products
-  checkout and getting total payment of his order
-  tracking his order using his order number
-  canceling his order but only for 3 days starting from checkout date
-  checking his notifications (sent after every operation)
-  adding review to products ( only he bought )
-  selling product
-  showing his on market products
-  recieving mails for every transactions


## Getting Started
- to use this API you should have SQL server / SQL management studio 
- just in the package manager console add this " update-database "
