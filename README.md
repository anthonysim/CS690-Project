# Library Branch Manager

A console app for managing a small library branch — built around the scenario of a librarian needing to know whether a book is available, check it out to a patron, keep study room bookings conflict-free, and run events without overbooking the room.

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
5. View library events
6. Check in to an event
7. Exit
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

### View library events
Choose `5` to list upcoming events with their date/time and current attendance, e.g. `2 of 2 attending`.

### Check in to an event
Choose `6`, then enter an Event ID and a Patron ID. Check-in is rejected once the event's attendance count reaches its capacity.

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

**Events**

| ID | Name | Capacity |
|----|------|----------|
| 1 | Children's Story Hour | 2 (small on purpose — easy to hit capacity with the 3 seeded patrons) |
| 2 | Author Talk: Local Writers | 20 |

### Persistence

Data is stored in pipe-delimited text files in the project root: `books.txt`, `patrons.txt`, `loans.txt`, `studyrooms.txt`, `reservations.txt`, `events.txt`, `eventattendance.txt` ([Storage/](Storage/)). They're loaded on startup and rewritten after every checkout, reservation, or check-in, so changes survive restarting the app. These files are gitignored — delete them to reset back to the seed data.

## Project structure

```
Models/      Domain entities (Book, Patron, Loan, StudyRoom, RoomReservation, Event, EventAttendance)
Services/    Business logic (BookService, LoanService, RoomReservationService, EventService)
Storage/     Text-file repositories (load/save each entity)
Data/        Seed data used when no data files exist yet
Program.cs   Console menu and entry point
```

## Current scope

This covers all eleven functional requirements from the original scenario:

- Search books by title, author, or ISBN
- Display whether a book is available, checked out, or unavailable
- Check out an available book to a patron
- Prevent borrowing if the patron has overdue items or a blocked account
- View overdue loans
- Reserve a study room for a patron, preventing double-booking
- View library events
- Track event attendance
- Prevent additional check-ins once an event is at capacity
- Store and retrieve library data from text files

Not yet implemented: returning books (no functional requirement covers this yet).
