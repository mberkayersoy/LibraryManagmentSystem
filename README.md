# Library Management System

This project is a simple library management system with the following features:

- **Add New Books:** Add new books to the library.
  - When adding a new book to the library, if the ISBN number of an existing book in the library is entered, the title and authors of the book to be added are compared with the existing book in the library. If the result of this comparison is the same, the number of existing books is increased, but if the comparison result is not the same, the user is asked to enter a different ISBN number.
- **Increase the Number of Existing Books:** Functionality to increase the quantity of existing books.
- **Borrow Books:** Users can borrow books from the library.
  - If all copies of the book to be borrowed have been lent, the user is not given the book and is asked to try again later.
  - If the user already has the book to be purchased, the book is not given to the user.
- **Search for Books:** Convenient search function to find books in the library.
- **Show All Books:** List all books available in the library.
- **Show All Borrowed Books:** View all books currently borrowed by users.
- **Show Overdue Books:** Identify and display books with overdue return dates.

### User Permissions

- **Member Login:** Users can log in as members. So, They can access all features in the library.
- **Guest Login:** Guest users have limited access to certain functions.
  - They cannot borrow books. As they cannot borrow books, they cannot return books.

**Note:** Guest users can perform all other operations that registered users can.

