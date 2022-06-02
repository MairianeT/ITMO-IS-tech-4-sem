package com.MyProject.lab2.Services;

import com.MyProject.lab2.Entities.Rabbit;
import com.MyProject.lab2.Repositories.RabbitRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;
import java.util.concurrent.atomic.AtomicLong;

@Service
public class RabbitServiceImpl implements RabbitService {

    private final RabbitRepository rabbitRepository;

    @Autowired
    public RabbitServiceImpl(RabbitRepository rabbitRepository){
        this.rabbitRepository = rabbitRepository;
    }

    @Override
    public void create(Rabbit rabbit) {
        rabbitRepository.save(rabbit);
    }

    @Override
    public List<Rabbit> readAll() {
        return new ArrayList<>(rabbitRepository.findAll());
    }

    @Override
    public Optional<Rabbit> read(long id) {
        return rabbitRepository.findById(id);
    }


    @Override
    public boolean delete(long id) {
        if (rabbitRepository.existsById(id)){
            rabbitRepository.deleteById(id);
            return true;
        }
        return false;
    }
}
