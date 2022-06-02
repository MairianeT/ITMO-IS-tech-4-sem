package com.MyProject.lab2.Services;

import com.MyProject.lab2.Entities.Rabbit;

import java.util.List;
import java.util.Optional;

public interface RabbitService {
    void create(Rabbit rabbit);
    List<Rabbit> readAll();
    Optional<Rabbit> read(long id);
    boolean delete(long id);
}
