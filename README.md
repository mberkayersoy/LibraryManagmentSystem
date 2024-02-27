# Library Management System

This project is a simple library management system with the following features:

- **Add New Books:** Add new books to the library.
  - When adding a new book, the system checks if the ISBN number already exists in the library. If so, it compares the title and authors of the new book with the existing one. If they match, the quantity of the existing book is increased. If not, the user is prompted to enter a different ISBN number.
- **Increase the Number of Existing Books:** Functionality to increase the quantity of existing books.
- **Borrow Books:** Users can borrow books from the library.
  - If all copies of the book are currently lent, the system denies the request and advises the user to try again later.
  - If the user already has a copy of the book being borrowed, the system does not provide an additional copy.
- **Search for Books:** Convenient search function to find books in the library.
- **Show All Books:** Displays a list of all available books in the library.
- **Show All Borrowed Books:** Displays all books currently borrowed by users.
- **Show Overdue Books:** Identify and display books with overdue return dates.

### User Permissions

- **Member Login:** Users can log in as members. So, They can access all features in the library.
- **Guest Login:** Guest users have limited access to certain functions.
  - They cannot borrow books. As they cannot borrow books, they cannot return books.

**Note:** Guest users can perform all other operations that registered users can.

# Runner Game

This project is a simple endless runner game with the following features:

**Controls**
- A-D: Side movement.
- W: Jump.
- S: Rolling while on the ground, and while in the air quick landing followed by roll .

**Random Obstacle Placement**
  - Each piece of ground that the character walks on is called chunk. Each chunk places its own obstacles randomly. While doing this placement, he always makes sure that there is a safe path for the character to pass. So it prevents the game from being unbeatable.
  - First of all, it moves through random lines on the chunk at random intervals and stores these intervals, and since these intervals will be safe intervals for the character, it does not place obstacles in these intervals. (If desired, passable obstacles can be added to these safe ranges.)
  - Then, it adds the coins that the character can collect to the pre-selected safe areas so that the coins that the character can collect do not become intertwined with obstacles.

**Selection With Weight**
  - Instead of using Random.Range() when creating obstacles or different chunks, a generic SelectorWithWeight class was developed. This class scans the Array elements given to it according to their weight values ​​and makes a random selection. However, in this selection process, the higher the weight value, the more likely the object is to be selected.
