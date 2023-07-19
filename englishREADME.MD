# Device Catalog

A CRUD application for mobile devices using Unity and Firebase.

## Description

This project is a mobile application developed with Unity and Firebase. Designed to meet the needs of an educational institution for managing their device inventory, the application allows for registering devices in a database and adding observations about each device. Additionally, the application features QR code and barcode scanning capabilities to capture asset numbers and IMEI of devices. The application also offers search, filtering, and data editing functionalities for the devices already stored in the database. Basic authentication with Firebase is implemented to ensure that users can access and modify device information.

## Features

- Mobile Device Registration: Users can add information about a mobile device, such as location, status, and serial number, to the database.

- Device Observations: Users can add specific observations or notes about each registered device, with a timestamp indicating when the observation was added to the database.

- QR Code and Barcode Scanning: The application is capable of scanning QR codes and barcodes to automatically capture asset numbers and IMEI of devices.

- Search and Filters: Users can perform searches and apply filters to find specific devices based on criteria such as IMEI number, status, whereabouts, and more.

- Data Editing and Deletion: Users can edit the information of devices already stored in the database, allowing for the update of device details.

- Basic Authentication with Firebase: User authentication is implemented to ensure that only authorized users can access their own data. A guest mode is available to access publicly available data for testing purposes.

- Storage Using Firestore: All device information and observations are stored using Firebase Firestore.

## System Requirements

- Android mobile device running Android 5.1 or higher (Requires testing).

## Installation and Setup

1. Download the project repository.

2. Open the project in Unity. ( Unity Ver.: 2022.3.3f1 )

3. Firebase Configuration:
   - Create a project in the Firebase Console (https://console.firebase.google.com/) and obtain the project credentials.
   - Add the project credentials to Unity following the Firebase SDK instructions.
   - Enable Firebase Firestore for data storage.
   - Configure basic authentication with Firebase Authentication.

4. Building the application:
   - Set the desired target platforms (Android, iOS) in the project settings in Unity.
   - Build and create the application installation package for the chosen platform.

5. Install the application on the mobile device.
