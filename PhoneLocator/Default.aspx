<%@ Page Language="C#" AutoEventWireup="true" Inherits="DisplayMap" Codebehind="Default.aspx.cs" %>

<!DOCTYPE html>
<html>
<head>
    <title>Google Map GPS Cell Phone Tracker</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta charset="utf-8">
 
    <script src="Scripts/jquery-3.1.1.min.js"></script>    
    <script src="Scripts/jquery.signalR-2.2.2.js" type="text/javascript" ></script>
    <script src="signalr/hubs" type="text/javascript" ></script>

    <link rel="stylesheet" href="Content/bootstrap.min.css" type="text/css">      
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.1.0/dist/leaflet.css" integrity="sha512-wcw6ts8Anuw10Mzh9Ytw4pylW8+NAD4ch3lqm9lzAsTxg0GFeJgoAtxuCLREZSC5lUXdVyo/7yfsqFjQ4S+aKw==" crossorigin=""/>
    <script src="https://unpkg.com/leaflet@1.1.0/dist/leaflet.js" integrity="sha512-mNqn2Wg7tSToJhvHcqfzLMU6J4mkOImSPTxVZAdo+lcPlk+GhZmYgACEe0x35K7YzW1zJ7XyJV/TT1MrdXvMcA==" crossorigin=""></script>
    <link rel="stylesheet" href="Content/styles.css" type="text/css">
    <script src="Scripts/maps.js"></script>
</head>
<body>
    <div id="map-canvas"></div>
</body>
</html>
    