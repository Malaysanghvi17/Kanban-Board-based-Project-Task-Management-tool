-- Create the lanes table if it doesn't exist
CREATE TABLE IF NOT EXISTS lane_columns (
    lid SERIAL PRIMARY KEY,
    bid INT NOT NULL,
    title VARCHAR(255) NOT NULL,
    label VARCHAR(255)
);

-- Insert sample data into lanes
INSERT INTO lane_columns (bid, title, label) VALUES
(1, 'To Do', 'Pending Tasks'),
(1, 'In Progress', 'Ongoing Tasks'),
(1, 'Done', 'Completed Tasks');
