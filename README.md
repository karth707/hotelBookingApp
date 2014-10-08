##HotelBookingApp
===============
Application for class assignment purposes.
Application that manages concurrent booking of hotel room by travel agents.

Description:

We have a scenario of Hotels and Travel agents who attempt booking of certain 'x' number of rooms based on price cuts in the hotels. The travel agencies are threads running concurrently and are subscribed to price cut events by the hotel suppliers.
The agents encode the order and place it in a multi-cell buffer for the hotels to read, decode and book the rooms. The hotel suppliers spawn threads for processing the order and send the confirmation back to the travel agent.

Class Diagram:

![alt tag](https://github.com/karth707/hotelBookingApp/blob/master/class-diagram.JPG)
