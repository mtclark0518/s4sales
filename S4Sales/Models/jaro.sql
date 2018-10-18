CREATE FUNCTION jaro_winkler_similarity(in1 varchar(255), in2 varchar(255)) 
    RETURNS float AS
BEGIN

--finestra:= search window, curString:= scanning cursor for the original string, curSub:= scanning cursor for the compared string

DECLARE finestra, curString, curSub, maxSub, trasposizioni, prefixlen, maxPrefix integer;
DECLARE char1, char2 char;
DECLARE common1, common2, old1, old2 varchar(255);
DECLARE trovato boolean;
DECLARE returnValue, jaro float;

SET maxPrefix = 6; 
--from the original jaro - winkler algorithm
SET common1 = "";
SET common2 = "";

SET finestra = 
    (LEN(in1) + LEN(in2) - ABS(LEN(in1) - LEN(in2))) / 4
    + ((LEN(in1) + LEN(in2) - ABS(LEN(in1) - LEN(in2))) / 2) % 2;

SET old1 = in1;
SET old2 = in2;

--calculating common letters vectors
SET curString = 1;

WHILE curString <= LEN(in1) and (curString <= (LEN(in2) + finestra))
BEGIN
    SET curSub = curstring - finestra;
    IF (curSub) < 1
    BEGIN 
        set curSub = 1;
    END;
    SET maxSub = curstring + finestra;
    IF (maxSub) > LEN(in2) 
    BEGIN 
        SET maxSub = LEN(in2);
    END;
    SET trovato = false;

    WHILE curSub <= maxSub AND trovato = false 
    BEGIN
        IF SUBSTRING(in1, curString, 1) = SUBSTRING(in2, curSub, 1) 
        BEGIN
            SET common1 = CONCAT(common1, SUBSTRING(in1, curString, 1));
            SET in2 = CONCAT(SUBSTRING(in2, 1, curSub - 1), CONCAT("0", SUBSTRING(in2, curSub + 1, LEN(in2) - curSub + 1)));
            SET trovato = true;
        END;
        SET curSub = curSub + 1;
    END;
    SET curString = curString + 1;
END;

--back to the original string
SET in2 = old2;
SET curString = 1;
while curString <= length(in2) AND (curString <= (length(in1) + finestra)) do
    SET curSub = curstring - finestra;
    if (curSub) < 1 then
        set curSub = 1;
    end if;
    SET maxSub = curstring + finestra;
    if (maxSub) > length(in1) then
        set maxSub = length(in1);
    end if;
    SET trovato = false;
    while curSub <= maxSub and trovato = false do
        if substr(in2, curString, 1) = substr(in1, curSub, 1) then
            SET common2 = concat(common2, substr(in2, curString, 1));
            SET in1 = concat(substr(in1, 1, curSub - 1),concat("0", substr(in1, curSub + 1,length(in1) - curSub + 1)));
            SET trovato = true;
        end if;
        SET curSub = curSub + 1;
    end while;
    SET curString = curString + 1;
end while;

--back to the original string
SET in1 = old1;


--calculating jaro metric
if length(common1) <> length(common2) 
    BEGIN
        SET jaro = 0;
    END
else if length(common1) = 0 or length(common2) = 0 
    BEGIN 
        SET jaro = 0;
    END
else
    BEGIN
        SET trasposizioni = 0;
        SET curString = 1;
        while curString <= length(common1) do
            if(substr(common1, curString, 1) <> substr(common2, curString, 1)) then
                set trasposizioni = trasposizioni + 1;

            end if;

            SET curString = curString + 1;

        end while;

        SET jaro = (length(common1) / length(in1) + length(common2) / length(in2)+ (length(common1) - trasposizioni/2) / length(common1)) / 3;
    END
end if; 

--calculating common prefix for winkler metric
SET prefixlen = 0;
while (substring(in1, prefixlen + 1, 1) = substring(in2, prefixlen + 1, 1)) and (prefixlen < 6) do
SET prefixlen = prefixlen +1;

end while;


--calculate jaro-winkler metric
return jaro + (prefixlen * 0.1 * (1 - jaro));
END