print("HELLO")

function init()
    addCmdLine("myName","myName")
end

function myName(this)
    console:addLog(tostring(getMaxPoint(0)))
    player:AddLog("Vanya")
    player:activespecial(0)
end

function useItem(Id)
    player:AddLog("Using User Item")
    return true
end

function Draw()
end

