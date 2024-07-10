# SimpleChat
 
 Description:
Users should be able to create chats, search for chats, connect to exist and delete if the user created the chat. When a user connects to chat, it should be able to write some text which will be visible for other users in the chat. 

Tech stack:
- Asp.Net Core (Web Api);
- Entity Framework Core (Code First);
- SignalR;

Architecture:
3-tier architecture.

Requirements:
- The whole tech stack has been used;
- Architecture used in the correct way;
- There is at least 1 api controller with CRUD operations;
- There is at least 1 SignlaR hub;
- There is at least 1 relationship in database has been used: one-to-one, one-to-many or many-to-many;
- There is at least 1 unit test (it’s up to you which library for testing you want to use);
- There is at least 1 integration test (it’s up to you which library for testing you want to use);
- Clean code;

Notes:
- Please, don’t create anything in Frontend (no need to use Asp.net Mvc or Blazor). As a user, I should be able to use your application through api calls/websockets.
- For testing purposes, you can use Postman/Fiddler or another tool.
- No need to use Identity or any authentications/authorizations. To distinguish users, you can pass some UserId in your request bodies.
- It’s ok to search for existing logic/source code but you have to be aware how logic is working, how to extend and maintain it.
- All business logic which you implement should work for different cases. For instance, as a user, I create the chat, connect to this one and try to delete it - all other users, who are connected to the chat, should be disconnected. Another case, as a user, I try to delete a chat which I didn’t create (it was created by another user) - I have to receive some error message like “There are no permissions to do the operation”.
