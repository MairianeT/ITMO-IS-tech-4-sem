package com.MyProject.lab2.Controllers;

import com.MyProject.lab2.Entities.Rabbit;
import com.MyProject.lab2.Services.RabbitService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/rabbits")
public class RabbitController {

    private final RabbitService rabbitService;

    @Autowired
    public RabbitController(RabbitService rabbitService) {
        this.rabbitService = rabbitService;
    }

    @PostMapping(value = "create")
    public void createRabbit(@RequestBody Rabbit rabbit) {
        rabbitService.create(rabbit);
    }

    @GetMapping(value = "get", produces = MediaType.APPLICATION_JSON_VALUE)
    public List<Rabbit> readAllRabbits() {
        final List<Rabbit> rabbits = rabbitService.readAll();
        return rabbits;
    }

    @GetMapping(value = "getId", produces = MediaType.APPLICATION_JSON_VALUE)
    public Optional<Rabbit> readByRabbitId(long id) {
        final Optional<Rabbit> rabbit = rabbitService.read(id);
        return rabbit;
    }
}

