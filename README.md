# Tournament GraphQL API 🏆

## ✅ Features
- **JWT Authentication:** Register, Login, and protected routes.
- **Tournament Logic:** Create tournaments, add participants, generate brackets.
- **In-Memory Database:** No external database setup required.

## 🚀 How to Run
1. Open the solution in Visual Studio.
2. Run the project.
3. Go to `https://localhost:YOUR_PORT/graphql/` (Banana Cake Pop).

## 🧪 Test Queries 

### 1. Register Users
```graphql
mutation {
  registerUser(firstName: "Robin", lastName: "Buldu", email: "robin@mail.com", password: "123") { id }
}
