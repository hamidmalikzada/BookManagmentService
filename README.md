# BookManagmentService

## Description
This project is a book management system built with .NET C# web api. It allows users to keep track of their books, including details such as title, authors, publisger and publication date.

## Features
- Add new books to the system
- Update details of existing books
- Delete books from the system
- Search for books by various parameters

## Installation
1. Clone the repository: `git clone https://github.com/hamidmalikzada/BookManagmentService.git`
2. Navigate to the project directory: `cd BookManagment`
3. Install the required dependencies.

## API Endpoints
Endpoints used in this project.

| Endpoint | HTTP Method | Description |
|----------|-------------|-------------|
| `/api/book` | `GET` | Fetch all books and its related data like authors and publisher |
| `/api/book` | `POST` | Add a new book |
| `/api/book/:id` | `GET` | Fetch a single book |
| `/api/book/:id` | `PUT` | Update a book |
| `/api/book/:id` | `DELETE` | Delete a book |
| `/api/author` | `GET` | Fetch all authors and its related data like books and publisher |
| `/api/author` | `POST` | Add a new author |
| `/api/author/:id` | `GET` | Fetch a single author |
| `/api/author/:id` | `PUT` | Update an author |
| `/api/author/:id` | `DELETE` | Delete an author |
| `/api/publisher` | `GET` | Fetch all publishers and it related data like books |
| `/api/publisher` | `POST` | Add a new publisher |
| `/api/publisher/:id` | `GET` | Fetch a single publisher |
| `/api/publisher/:id` | `PUT` | Update a publisher |
| `/api/publisher/:id` | `DELETE` | Delete a publisher |
| `/api/user` | `GET` | Fetch all users |
| `/api/user` | `POST` | Add a new user |
| `/api/user/:id` | `GET` | Fetch a single user |
| `/api/user/:id` | `PUT` | Update a user |
| `/api/user/:id` | `DELETE` | Delete a user |
| `/api/authentication` | `POST` | Returns an authentication token |

## Database Schema

### Book
- id: ObjectId
- title: String
- category: enum
- authors: Array of Objects (references Author)
- publisher: ObjectId (references Publisher)

### Author
- id: ObjectId
- name: String
- books: Array of Objects (references Book)

### Publisher
- id: ObjectId
- name: String
- books: Array of Objects (references Book)

### User
- id: ObjectId
- username: String
- password: String (hashed)



