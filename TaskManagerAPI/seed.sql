-- Drop existing if needed
DROP TABLE IF EXISTS TagTaskItem;
DROP TABLE IF EXISTS Tags;
DROP TABLE IF EXISTS Tasks;

-- Recreate tables

CREATE TABLE Tags (
                      Id INT AUTO_INCREMENT PRIMARY KEY,
                      Name LONGTEXT NOT NULL
);

CREATE TABLE Tasks (
                       Id INT AUTO_INCREMENT PRIMARY KEY,
                       Title LONGTEXT NOT NULL,
                       Description LONGTEXT,
                       CreatedAt DATETIME(6) NOT NULL,
                       IsCompleted TINYINT(1) NOT NULL
);

CREATE TABLE TagTaskItem (
                             TagsId INT NOT NULL,
                             TasksId INT NOT NULL,
                             PRIMARY KEY (TagsId, TasksId),
                             FOREIGN KEY (TagsId) REFERENCES Tags(Id),
                             FOREIGN KEY (TasksId) REFERENCES Tasks(Id)
);

-- Insert Tags

INSERT INTO Tags (Name) VALUES
                            ('Urgent'), ('Client'), ('Work'), ('Personal'), ('Today'), ('Weekly'), ('Learning'), ('Health');

-- Insert Tasks

INSERT INTO Tasks (Title, Description, CreatedAt, IsCompleted) VALUES
                                                                   ('Update Resume', 'Include latest project at Intuit', NOW(), 0),
                                                                   ('Meeting with Client A', 'Review quarterly progress', NOW(), 0),
                                                                   ('Doctor Appointment', 'Annual physical checkup', NOW(), 1),
                                                                   ('Buy Groceries', 'Milk, Eggs, Fruits', NOW(), 1),
                                                                   ('Clean Desktop', 'Organize files and folders', NOW(), 0),
                                                                   ('Watch .NET video', 'Complete the Entity Framework tutorial', NOW(), 0),
                                                                   ('Prepare Demo', 'Build presentation for task manager', NOW(), 0),
                                                                   ('Yoga session', 'Evening stretch for 30 min', NOW(), 1);

-- Insert Tags to Tasks (many-to-many)

INSERT INTO TagTaskItem (TagsId, TasksId) VALUES
                                              (1, 1), -- Urgent -> Update Resume
                                              (3, 1), -- Work -> Update Resume
                                              (2, 2), -- Client -> Meeting
                                              (3, 2), -- Work -> Meeting
                                              (8, 3), -- Health -> Doctor
                                              (4, 4), -- Personal -> Groceries
                                              (4, 5), -- Personal -> Clean Desktop
                                              (7, 6), -- Learning -> Video
                                              (3, 6), -- Work -> Video
                                              (3, 7), -- Work -> Demo
                                              (1, 7), -- Urgent -> Demo
                                              (8, 8); -- Health -> Yoga