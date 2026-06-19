# Library Branch Manager

A console app for managing a small library branch — built around the scenario of a librarian needing to know whether a book is available and check it out to a patron.

## Requirements

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) or later

## Running

```bash
dotnet run
```

You'll see a menu:

```
=== Library Branch Manager ===
1. Search books
2. Check out a book
3. Exit
```

### Search books
Choose `1`, then enter a search term. It matches against title, author, or ISBN (case-insensitive, partial match). Leave it blank to list every book in the catalog. Each result shows its status — `Available`, `Checked Out`, or `Unavailable` — and the copy count, e.g.:

```
[1] The Midnight Library by Matt Haig — Checked Out (0 of 1 available)
[2] Atomic Habits by James Clear — Available (2 of 3 available)
```

### Check out a book
Choose `2`, then enter a Patron ID and a Book ID. If the book has copies available, it's checked out and you'll see a confirmation with a due date 14 days out. If no copies are available (or the patron/book ID doesn't exist), you'll see an error message instead.

### Sample data

The app seeds the following in-memory data on every run ([Data/SeedData.cs](Data/SeedData.cs)):

**Books**

| ID | Title | Author | Status |
|----|-------|--------|--------|
| 1 | The Midnight Library | Matt Haig | Checked Out (0/1) |
| 2 | Atomic Habits | James Clear | Available (2/3) |
| 3 | Where the Crawdads Sing | Delia Owens | Available (1/2) |

**Patrons**

| ID | Name |
|----|------|
| 1 | Maria Lopez |
| 2 | James Carter |

> Data is in-memory only and resets every time the app restarts — there is no persistence yet.

## Project structure

```
Models/      Domain entities (Book, Patron, Loan)
Services/    Business logic (BookService for search, LoanService for checkout)
Data/        In-memory seed data
Program.cs   Console menu and entry point
```

## Current scope

This is Iteration 1 of the project, covering three functional requirements:

- Search books by title, author, or ISBN
- Display whether a book is available, checked out, or unavailable
- Check out an available book to a patron

Not yet implemented: overdue/blocked-patron restrictions, returning books, overdue loan views, study room reservations, library events, and file-based persistence.
