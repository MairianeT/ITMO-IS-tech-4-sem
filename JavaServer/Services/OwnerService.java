package com.MyProject.lab2.Services;

import com.MyProject.lab2.Entities.Owner;
import com.MyProject.lab2.Entities.Rabbit;

import java.util.List;
import java.util.Optional;

public interface OwnerService {
    void create(Owner owner);
    List<Owner> readAll();
    Optional<Owner> read(long id);
    boolean delete(long id);
}
