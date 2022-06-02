package com.MyProject.lab2.Repositories;

import com.MyProject.lab2.Entities.Rabbit;
import org.springframework.data.jpa.repository.JpaRepository;

public interface RabbitRepository extends JpaRepository<Rabbit, Long> {
}
