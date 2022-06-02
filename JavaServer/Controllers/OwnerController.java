package com.MyProject.lab2.Controllers;
import com.MyProject.lab2.Entities.Owner;
import com.MyProject.lab2.Services.OwnerService;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/owners")
public class OwnerController {
    private final OwnerService ownerService;
    public OwnerController(OwnerService ownerService){
        this.ownerService = ownerService;
    }

    @PostMapping(value = "create")
    public void createOwner(@RequestBody Owner owner) {
        ownerService.create(owner);
    }

    @GetMapping(value = "get", produces = MediaType.APPLICATION_JSON_VALUE)
    public List<Owner> readAllOwners() {
        final List<Owner> owners = ownerService.readAll();
        return owners;
    }

    @GetMapping(value = "getId", produces = MediaType.APPLICATION_JSON_VALUE)
    public Optional<Owner> readByOwnerId(long id) {
        final Optional<Owner> owner = ownerService.read(id);
        return owner;
    }
}
