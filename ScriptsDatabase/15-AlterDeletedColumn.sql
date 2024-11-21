alter table Company
	add Deleted bit

update Company set Deleted = 0