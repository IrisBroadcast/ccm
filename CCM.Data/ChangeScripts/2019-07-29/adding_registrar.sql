ALTER TABLE `uccm`.`registeredsips` 
ADD COLUMN `Registrar` LONGTEXT NULL DEFAULT NULL AFTER `User_UserId`;