score = 0

function start()
    log("Démarrage de " .. appName)
end

function update(dt)
    score = score + 1
    return add(score, dt)
end
