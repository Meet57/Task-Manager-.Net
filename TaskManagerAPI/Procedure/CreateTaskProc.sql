
DELIMITER //
CREATE PROCEDURE CreateTaskWithTags (IN p_title VARCHAR(255), IN p_description TEXT, IN p_isCompleted BOOLEAN, IN p_tags TEXT) BEGIN DECLARE task_id INT; DECLARE one_tag VARCHAR(255); DECLARE comma_index INT; DECLARE tag_id INT; -- Insert the Task

INSERT INTO Tasks (Title, Description, IsCompleted, CreatedAt)
VALUES (p_title,
        p_description,
        p_isCompleted,
        UTC_TIMESTAMP());
SET task_id = LAST_INSERT_ID(); -- Loop over comma-separated tags
 WHILE LENGTH(p_tags) > 0 DO
SET comma_index = LOCATE(',', p_tags); IF comma_index > 0 THEN
SET one_tag = TRIM(SUBSTRING(p_tags, 1, comma_index - 1));
SET p_tags = TRIM(SUBSTRING(p_tags, comma_index + 1)); ELSE
SET one_tag = TRIM(p_tags);
SET p_tags = ''; END IF;
INSERT
IGNORE INTO Tags (Name)
VALUES (one_tag);
SELECT Id INTO tag_id
FROM Tags
WHERE Name = one_tag
    LIMIT 1;
INSERT INTO TagTaskItem (TasksId, TagsId)
VALUES (task_id,
        tag_id); END WHILE;
SELECT task_id AS NewTaskId; END //
DELIMITER ;