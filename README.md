# Welcome to the Where We 'Bout To Eat App

## Overview
The Where We Bout to Eat App is a Progressive Web Application built with the Blazor Framework for building web applications and uses a .NET Core Web API to communicate with a SQL Database and external APIs. The goal of the application is to implicitly and explicitly gather user data and recommend food-oriented suggestions (E.g. recipes, restaurants, etc.) based on that data.

## Installation Guide
Installation isn't necessary to preview the functionality of this prototype. The version of the Where We 'Bout To Eat App that is stored on this branch is currently being hosted at https://wherewebouttoeat.azurewebsites.net/ 

## Basic Workflow
This prototype will allow the user to create a user account. Once the account is created, the user will be able to select the types of recipes they perfer, search for recipes, add and edit list of favorite recipes and be recommended recipes.

## Technical Details
The recommender system that will be used to recommend recipes is a hybrid solution. The recipes are external sourced using API calls from the application .NET Core API. The sole recipe source for the prototype is the Tasty API developed by RapidAPI. As recipes are sourced from the Tasty API, there details are imported into the application’s SQL database. The application then uses the user’s data to query recipes from the SQL database that match their preferences and/or favorites. The application then uses a Rank API from Azure Cognitive Services that will rank the recipes based on its algorithm. The application will sort the recipes in order of the Rank API’s ranking If the user selects the API top choice, the application communicates back to the Rank API which allows the algorithm to learn. 
