# Library Branch Manager

A console app for managing a small library branch — built around the scenario of a librarian needing to know whether a book is available, check it out to a patron, and keep study room bookings conflict-free.

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
3. View overdue loans
4. Reserve a study room
5. Exit
```

### Search books
Choose `1`, then enter a search term. It matches against title, author, or ISBN (case-insensitive, partial match). Leave it blank to list every book in the catalog. Each result shows its status — `Available`, `Checked Out`, or `Unavailable` — and the copy count, e.g.:

```
[1] The Midnight Library by Matt Haig — Checked Out (0 of 1 available)
[2] Atomic Habits by James Clear — Available (2 of 3 available)
```

### Check out a book
Choose `2`, then enter a Patron ID and a Book ID. Checkout is blocked if:
- the patron's account is blocked, or
- the patron has any overdue loan, or
- the book has no available copies.

Otherwise the book is checked out and you'll see a confirmation with a due date 14 days out.

### View overdue loans
Choose `3` to list every loan past its due date, with the patron, book, due date, and days overdue.

### Reserve a study room
Choose `4`, then enter a Patron ID, Room ID, date (`yyyy-MM-dd`), start time, and end time (`HH:mm`). The reservation is rejected if it overlaps an existing booking for that room on that date.

### Sample data

On first run (when the data files below don't exist yet), the app seeds the following data and writes it to text files ([Data/SeedData.cs](Data/SeedData.cs)):

**Books**

| ID | Title | Author | Status |
|----|-------|--------|--------|
| 1 | The Midnight Library | Matt Haig | Checked Out (0/1) |
| 2 | Atomic Habits | James Clear | Available (2/3) |
| 3 | Where the Crawdads Sing | Delia Owens | Available (1/2) |

**Patrons**

| ID | Name | Notes |
|----|------|-------|
| 1 | Maria Lopez | Has an overdue loan (Book 3, due 6 days ago) |
| 2 | James Carter | |
| 3 | Sam Reed | Blocked account |

**Study Rooms**

| ID | Name |
|----|------|
| 1 | Room A |
| 2 | Room B |

### Persistence

Data is stored in pipe-delimited text files in the project root: `books.txt`, `patrons.txt`, `loans.txt`, `studyrooms.txt`, `reservations.txt` ([Storage/](Storage/)). They're loaded on startup and rewritten after every checkout or reservation, so changes survive restarting the app. These files are gitignored — delete them to reset back to the seed data.

## Project structure

```
Models/      Domain entities (Book, Patron, Loan, StudyRoom, RoomReservation)
Services/    Business logic (BookService, LoanService, RoomReservationService)
Storage/     Text-file repositories (load/save each entity)
Data/        Seed data used when no data files exist yet
Tests/       xUnit tests for the Services
Program.cs   Console menu and entry point
```

## Tests

Unit tests cover the three service modules:

- **BookServiceTests** — catalog search (blank term, title/author/ISBN match, case-insensitivity, no match)
- **LoanServiceTests** — checkout success/failure paths (no copies, blocked patron, overdue patron) and overdue loan filtering
- **RoomReservationServiceTests** — reservation success/failure paths (bad time range, overlapping booking, unknown room)

Run them with:

```bash
cd Tests
dotnet test
```

## Current scope

This covers seven functional requirements:

- Search books by title, author, or ISBN
- Display whether a book is available, checked out, or unavailable
- Check out an available book to a patron
- Store and retrieve library data from text files
- Prevent borrowing if the patron has overdue items or a blocked account
- View overdue loans
- Reserve a study room for a patron, preventing double-booking

Not yet implemented: returning books, library events.
