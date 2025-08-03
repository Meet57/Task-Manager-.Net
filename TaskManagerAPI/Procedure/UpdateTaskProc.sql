
DELIMITER //
CREATE PROCEDURE UpdateTaskWithTag (IN p_id INT, IN p_title VARCHAR(255), IN p_description TEXT, IN p_isCompleted BOOLEAN, IN p_tags TEXT) BEGIN DECLARE one_tag VARCHAR(255); DECLARE comma_index INT; DECLARE tag_id INT; -- Update Task

UPDATE Tasks
SET Title = p_title,
    Description = p_description,
    IsCompleted = p_isCompleted
WHERE Id = p_id; -- Remove old tag mappings

DELETE
FROM TagTaskItem WHERE TasksId = p_id; -- Re-add tags
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
VALUES (p_id,
        tag_id); END WHILE;
SELECT p_id AS UpdatedTaskId; END //
DELIMITER ;