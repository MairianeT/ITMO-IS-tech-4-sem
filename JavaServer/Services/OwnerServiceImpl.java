package com.MyProject.lab2.Services;

import com.MyProject.lab2.Entities.Owner;
import com.MyProject.lab2.Repositories.OwnerRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@Service
public class OwnerServiceImpl implements OwnerService{
    private final OwnerRepository ownerRepository;

    @Autowired
    public OwnerServiceImpl(OwnerRepository ownerRepository, RabbitService rabbitService){
        this.ownerRepository = ownerRepository;
    }
    @Override
    public void create(Owner owner) {
        ownerRepository.save(owner);
    }

    @Override
    public List<Owner> readAll() {
        return new ArrayList<>(ownerRepository.findAll());
    }

    @Override
    public Optional<Owner> read(long id) {
        return ownerRepository.findById(id);
    }

    @Override
    public boolean delete(long id) {
        if (ownerRepository.existsById(id)) {
            ownerRepository.deleteById(id);
            return true;
        }
        return false;
    }
}
