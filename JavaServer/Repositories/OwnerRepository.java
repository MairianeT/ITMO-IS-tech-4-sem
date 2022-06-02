package com.MyProject.lab2.Repositories;

import com.MyProject.lab2.Entities.Owner;
import org.springframework.data.jpa.repository.JpaRepository;

public interface OwnerRepository extends JpaRepository<Owner, Long> {
}
